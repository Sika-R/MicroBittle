let showGear = false
let gear = 0
let isHalf = false
class MicrobitEventType {
    static Connected: number
    private ___Connected_is_set: boolean
    private ___Connected: number
    get Connected(): number {
        return this.___Connected_is_set ? this.___Connected : MicrobitEventType.Connected
    }
    set Connected(value: number) {
        this.___Connected_is_set = true
        this.___Connected = value
    }
    
    static Disconnected: number
    private ___Disconnected_is_set: boolean
    private ___Disconnected: number
    get Disconnected(): number {
        return this.___Disconnected_is_set ? this.___Disconnected : MicrobitEventType.Disconnected
    }
    set Disconnected(value: number) {
        this.___Disconnected_is_set = true
        this.___Disconnected = value
    }
    
    static ButtonAPressed: number
    private ___ButtonAPressed_is_set: boolean
    private ___ButtonAPressed: number
    get ButtonAPressed(): number {
        return this.___ButtonAPressed_is_set ? this.___ButtonAPressed : MicrobitEventType.ButtonAPressed
    }
    set ButtonAPressed(value: number) {
        this.___ButtonAPressed_is_set = true
        this.___ButtonAPressed = value
    }
    
    static ButtonBPressed: number
    private ___ButtonBPressed_is_set: boolean
    private ___ButtonBPressed: number
    get ButtonBPressed(): number {
        return this.___ButtonBPressed_is_set ? this.___ButtonBPressed : MicrobitEventType.ButtonBPressed
    }
    set ButtonBPressed(value: number) {
        this.___ButtonBPressed_is_set = true
        this.___ButtonBPressed = value
    }
    
    static P0: number
    private ___P0_is_set: boolean
    private ___P0: number
    get P0(): number {
        return this.___P0_is_set ? this.___P0 : MicrobitEventType.P0
    }
    set P0(value: number) {
        this.___P0_is_set = true
        this.___P0 = value
    }
    
    static P1: number
    private ___P1_is_set: boolean
    private ___P1: number
    get P1(): number {
        return this.___P1_is_set ? this.___P1 : MicrobitEventType.P1
    }
    set P1(value: number) {
        this.___P1_is_set = true
        this.___P1 = value
    }
    
    static P2: number
    private ___P2_is_set: boolean
    private ___P2: number
    get P2(): number {
        return this.___P2_is_set ? this.___P2 : MicrobitEventType.P2
    }
    set P2(value: number) {
        this.___P2_is_set = true
        this.___P2 = value
    }
    
    public static __initMicrobitEventType() {
        MicrobitEventType.Connected = 0
        MicrobitEventType.Disconnected = 1
        MicrobitEventType.ButtonAPressed = 2
        MicrobitEventType.ButtonBPressed = 3
        MicrobitEventType.P0 = 4
        MicrobitEventType.P1 = 5
        MicrobitEventType.P2 = 6
    }
    
}

MicrobitEventType.__initMicrobitEventType()

function SendMessage(msgType: number, msg: string) {
    let prefix = "" + msgType
    serial.writeLine(prefix + msg)
}

function ReceiveMessage(msg: string) {
    let line = serial.readLine()
    line = line.slice(1)
    let prefix = line.charAt(0)
    let prefixValue = parseInt(prefix)
}

let readLine = ""
//  serial.write_line("hi")
input.onButtonPressed(Button.A, function on_button_pressed_a() {
    
    gear = (gear + 3) % 4
    SendMessage(MicrobitEventType.ButtonAPressed, "hi")
})
//  serial.write_line("hi")
input.onButtonPressed(Button.B, function on_button_pressed_b() {
    
    gear = (gear + 1) % 4
    SendMessage(MicrobitEventType.ButtonBPressed, "hi")
})
serial.onDataReceived(serial.delimiters(Delimiters.CarriageReturn), function on_data_received() {
    
    //  serial.write_line(serial.read_line())
    readLine = serial.readLine().charAt(0)
    //  print(readLine)
    // if readLine == "s":
    
    showGear = true
    
    gear = 0
})
basic.forever(function on_forever() {
    
    
    
    
    //  SendMessage(MicrobitEventType.LightLvl, "" + str((input.light_level())))
    //  serial.write_line("" + str((input.light_level())))
    //  direction = parse_accelerometer()
    //  SendMessage(MicrobitEventType.accelerometer, "" + str(direction))
    //  x = input.acceleration(Dimension.X)
    //  y = input.acceleration(Dimension.Y)
    let P0 = parse_P0()
    let P1 = parse_P1()
    let P2 = parse_P2()
    //  print(humid)
    //  print(slider)
    if (isHalf) {
        SendMessage(MicrobitEventType.P1, "" + ("" + P1))
        SendMessage(MicrobitEventType.P2, "" + ("" + P2))
        
        if (showGear) {
            ShowGear(gear)
        }
        
    }
    
    SendMessage(MicrobitEventType.P0, "" + ("" + P0))
    isHalf = !isHalf
    basic.pause(250)
})
