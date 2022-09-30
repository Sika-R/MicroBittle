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
    
    static LightLvl: number
    private ___LightLvl_is_set: boolean
    private ___LightLvl: number
    get LightLvl(): number {
        return this.___LightLvl_is_set ? this.___LightLvl : MicrobitEventType.LightLvl
    }
    set LightLvl(value: number) {
        this.___LightLvl_is_set = true
        this.___LightLvl = value
    }
    
    public static __initMicrobitEventType() {
        MicrobitEventType.Connected = 0
        MicrobitEventType.Disconnected = 1
        MicrobitEventType.ButtonAPressed = 2
        MicrobitEventType.ButtonBPressed = 3
        MicrobitEventType.LightLvl = 4
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
    SendMessage(MicrobitEventType.ButtonAPressed, "hi")
})
serial.onDataReceived(serial.delimiters(Delimiters.CarriageReturn), function on_data_received() {
    
    serial.writeLine(serial.readLine())
    readLine = serial.readLine().charAt(0)
})
basic.forever(function on_forever() {
    
    if (readLine == "u") {
        basic.showLeds(`
            . . # . .
                        . # # # .
                        # . # . #
                        . . # . .
                        . . # . .
        `)
        basic.pause(200)
        readLine = ""
        basic.showLeds(`
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        `)
    } else if (readLine == "d") {
        basic.showLeds(`
            . . # . .
                        . . # . .
                        # . # . #
                        . # # # .
                        . . # . .
        `)
        basic.pause(200)
        readLine = ""
        basic.showLeds(`
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        `)
    } else if (readLine == "l") {
        basic.showLeds(`
            . . # . .
                        . # . . .
                        # # # # #
                        . # . . .
                        . . # . .
        `)
        basic.pause(200)
        readLine = ""
        basic.showLeds(`
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        `)
    } else if (readLine == "r") {
        basic.showLeds(`
            . . # . .
                        . . . # .
                        # # # # #
                        . . . # .
                        . . # . .
        `)
        basic.pause(200)
        readLine = ""
        basic.showLeds(`
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        `)
    } else if (readLine == "b") {
        basic.showString("B")
        basic.pause(200)
        readLine = ""
        basic.showLeds(`
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        `)
    } else if (readLine == "a") {
        basic.showString("A")
        basic.pause(200)
        readLine = ""
        basic.showLeds(`
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        `)
    } else if (readLine == "s") {
        basic.showString("START")
        basic.pause(200)
        readLine = ""
        basic.showLeds(`
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        `)
    }
    
    SendMessage(MicrobitEventType.LightLvl, "" + ("" + input.lightLevel()))
    //  serial.write_line("" + str((input.light_level())))
    basic.pause(100)
})
