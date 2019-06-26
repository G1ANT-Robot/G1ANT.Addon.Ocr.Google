# timeoutocr

## Syntax

```G1ANT
♥timeoutocr = ⟦timespan⟧
```

## Description

Determines the timeout value (in ms) for the `ocrgoogle.` and the `ocrtesseract.` commands; the default value is 60000 (60 seconds).

## Example

```G1ANT
♥timeoutocr = 100
♥googleLogin = Provide your Google Cloud credential here
ocrgoogle.login ♥googleLogin
program notepad
keyboard ⋘ENTER⋙‴   TEST    ‴
delay 1
ocrgoogle.find search test
dialog ♥result
```

In the example above the 100ms timeout is not enough to complete text recognition.
