showGear = False
gear = 0
isHalf = False

class MicrobitEventType(Enum):
    Connected = 0
    Disconnected = 1
    ButtonAPressed = 2
    ButtonBPressed = 3
    P0 = 4
    P1 = 5
    P2 = 6

def SendMessage(msgType, msg):
    prefix = str(msgType)
    serial.write_line(prefix + msg)
    
def ReceiveMessage(msg : str):
    line = serial.read_line()
    line = line[1:]
    prefix = line.char_at(0)
    prefixValue = int(prefix)

readLine = ""

def on_button_pressed_a():
    global gear
    gear = (gear + 3) % 4
    SendMessage(MicrobitEventType.ButtonAPressed, "hi")
    # serial.write_line("hi")
input.on_button_pressed(Button.A, on_button_pressed_a)

def on_button_pressed_b():
    global gear
    gear = (gear + 1) % 4
    SendMessage(MicrobitEventType.ButtonBPressed, "hi")
    # serial.write_line("hi")
input.on_button_pressed(Button.B, on_button_pressed_b)

def on_data_received():
    global readLine
    # serial.write_line(serial.read_line())
    readLine = serial.read_line().char_at(0)
    # print(readLine)
    #if readLine == "s":
    global showGear
    showGear = True
    global gear
    gear = 0

serial.on_data_received(serial.delimiters(Delimiters.CARRIAGE_RETURN),
    on_data_received)

def on_forever():
    global readLine
    global showGear
    global gear
    global isHalf
    
    # SendMessage(MicrobitEventType.LightLvl, "" + str((input.light_level())))
    # serial.write_line("" + str((input.light_level())))
    # direction = parse_accelerometer()
    # SendMessage(MicrobitEventType.accelerometer, "" + str(direction))
    # x = input.acceleration(Dimension.X)
    # y = input.acceleration(Dimension.Y)
    P0 = parse_P0()
    P1 = parse_P1()
    P2 = parse_P2()
    # print(humid)
    # print(slider)
    if(isHalf):
        SendMessage(MicrobitEventType.P1, "" + str(P1))
        SendMessage(MicrobitEventType.P2, "" + str(P2))
        global showGear
        if showGear:
            ShowGear(gear)
    SendMessage(MicrobitEventType.P0, "" + str(P0))
    isHalf = not isHalf
    basic.pause(250)
basic.forever(on_forever)
