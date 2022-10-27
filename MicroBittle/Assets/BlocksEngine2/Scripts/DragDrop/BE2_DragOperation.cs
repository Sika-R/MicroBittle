using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.UI;
using MG_BlocksEngine2.Environment;

namespace MG_BlocksEngine2.DragDrop
{
    public class BE2_DragOperation : MonoBehaviour, I_BE2_Drag
    {
        BE2_DragDropManager _dragDropManager;
        RectTransform _rectTransform;
        I_BE2_Spot _usedSpot;
        Transform _transform;
        public Transform Transform => _transform ? _transform : transform;
        public Vector2 RayPoint => _rectTransform.position;
        public I_BE2_Block Block { get; set; }
        public List<I_BE2_Block> ChildBlocks { get; set; }

        void Awake()
        {
            _transform = transform;
            _rectTransform = GetComponent<RectTransform>();
            Block = GetComponent<I_BE2_Block>();
        }

        void Start()
        {
            _dragDropManager = BE2_DragDropManager.Instance;
        }

        //void Update()
        //{
        //
        //}

        public void OnPointerDown()
        {
            _dragDropManager = BE2_DragDropManager.Instance;

            ChildBlocks = new List<I_BE2_Block>();
            ChildBlocks.AddRange(GetComponentsInChildren<I_BE2_Block>());
        }

        public void OnRightPointerDownOrHold()
        {
            BE2_UI_ContextMenuManager.instance.OpenContextMenu(0, Block);
        }

        public void OnDrag()
        {
            if (_usedSpot != null)
            {
                _usedSpot.Transform.SetSiblingIndex(Transform.GetSiblingIndex());
                _usedSpot.Transform.gameObject.SetActive(true);
                _usedSpot = null;
            }

            if (Transform.parent != _dragDropManager.DraggedObjectsTransform)
                Transform.SetParent(_dragDropManager.DraggedObjectsTransform, true);

            // v2.6 - bugfix: fixed operation blocks not using drag ad drop detection distance as parameter 
            I_BE2_Spot spot = _dragDropManager.Raycaster.FindClosestSpotOfType<BE2_SpotBlockInput>(this, _dragDropManager.detectionDistance);

            if (spot != null)
            {
                // last selected spot
                if (_dragDropManager.CurrentSpot != null && _dragDropManager.CurrentSpot != spot)
                    (_dragDropManager.CurrentSpot as BE2_SpotBlockInput).outline.enabled = false;

                _dragDropManager.CurrentSpot = spot;
                (_dragDropManager.CurrentSpot as BE2_SpotBlockInput).outline.enabled = true;
            }
            else
            {
                if (_dragDropManager.CurrentSpot != null)
                {
                    (_dragDropManager.CurrentSpot as BE2_SpotBlockInput).outline.enabled = false;
                    _dragDropManager.CurrentSpot = null;
                }
            }
        }

        public void OnPointerUp()
        {
            if (_dragDropManager.CurrentSpot != null)
            {
                Transform.SetParent(_dragDropManager.CurrentSpot.Transform.parent);
                Transform.SetSiblingIndex(_dragDropManager.CurrentSpot.Transform.GetSiblingIndex());

                (_dragDropManager.CurrentSpot as BE2_SpotBlockInput).outline.enabled = false;
                _usedSpot = _dragDropManager.CurrentSpot;
                _usedSpot.Transform.gameObject.SetActive(false);

                _dragDropManager.CurrentSpot = null;
            }
            else
            {
                I_BE2_Spot spot = _dragDropManager.Raycaster.GetSpotAtPosition(RayPoint);
                if (spot != null)
                {
                    I_BE2_ProgrammingEnv programmingEnv = spot.Transform.GetComponentInParent<I_BE2_ProgrammingEnv>();
                    if (programmingEnv == null && spot.Transform.GetChild(0) != null)
                        programmingEnv = spot.Transform.GetChild(0).GetComponentInParent<I_BE2_ProgrammingEnv>();

                    if (programmingEnv != null)
                        Transform.SetParent(programmingEnv.Transform);
                    else
                        Destroy(Transform.gameObject);
                }
                else
                {
                    Destroy(Transform.gameObject);
                }
            }

            // v2.6 - adjustments on position and angle of blocks for supporting all canvas render modes
            Transform.localPosition = new Vector3(Transform.localPosition.x, Transform.localPosition.y, 0);
            Transform.localEulerAngles = Vector3.zero;

            // v2.9 - bugfix: TargetObject of blocks being null
            Block.Instruction.InstructionBase.UpdateTargetObject();
        }

        // v2.1 - bugfix: fixed destroying operations placed as inputs causing error 
        void OnDisable()
        {
            if (_usedSpot != null)
            {
                // v2.3 - bigfix: fixed intermittent error "cannot change sibling OnDisable"
                _usedSpot.Transform.gameObject.SetActive(true);
                _usedSpot = null;
            }

            if (Transform.parent != _dragDropManager.DraggedObjectsTransform)
                Transform.gameObject.SetActive(false);
        }
    }
}
