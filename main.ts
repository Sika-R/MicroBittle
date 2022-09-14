let readLine = ""
serial.onDataReceived(serial.delimiters(Delimiters.CarriageReturn), function on_data_received() {
    
    serial.writeLine(serial.readLine())
    readLine = serial.readLine().charAt(0)
})
input.onButtonPressed(Button.A, function on_button_pressed_a() {
    serial.writeLine("hi")
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
    
    serial.writeLine("" + input.lightLevel())
    basic.pause(100)
})
