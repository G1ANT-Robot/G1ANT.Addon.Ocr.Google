# ocrgoogle.findpoint

## Syntax

```G1ANT
ocrgoogle.findpoint search ⟦text⟧ area ⟦rectangle⟧ relative ⟦bool⟧ 
```

## Description

This command finds a specified text on the active screen and returns its position in a [point](G1ANT.Robot/G1ANT.Language/G1ANT.Language/Structures/PointStructure.md) format. The point will be set in the middle of found text, i.e. if the text was found in rectangular area 0⫽0⫽20⫽20, the result will be 10⫽10.

| Argument       | Type                                                         | Required | Default Value                                                | Description                                                  |
| -------------- | ------------------------------------------------------------ | -------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| `search`       | [text](G1ANT.Language/G1ANT.Language/Structures/TextStructure.md) | yes      |                                                              | Text to be found on the screen (the fewer words, the better results) |
| `area`         | [rectangle](G1ANT.Robot/G1ANT.Language/G1ANT.Language/Structures/RectangleStructure.md) | no       | (equal to the current screen area)                           | Area on the screen to find text in, specified in `x0⫽y0⫽x1⫽y1` format, where `x0⫽y0` are the coordinates of the top left and `x1⫽y1` are the coordinates of the right bottom corner of the area |
| `relative`     | [bool](G1ANT.Language/G1ANT.Language/Structures/BooleanStructure.md) | no       | true                                                         | Determines whether the `area` argument is specified with absolute coordinates (top left corner of the screen) or refers to the currently opened window (its top left corner) |
| `lang`         | [text](G1ANT.Language/G1ANT.Language/Structures/TextStructure.md) | no       | en                                                           | List of languages (comma separated) to be used for text recognition |
| `result`       | [variable](G1ANT.Language/G1ANT.Language/Structures/VariableStructure.md) | no       | `♥result`                                                    | Name of a variable where the command's result will be stored |
| `if`           | [bool](G1ANT.Language/G1ANT.Language/Structures/BooleanStructure.md) | no       | true                                                         | Executes the command only if a specified condition is true   |
| `timeout`      | [timespan](G1ANT.Language/G1ANT.Language/Structures/TimeSpanStructure.md) | no       | [♥timeoutocr](G1ANT.Addon/G1ANT.Addon.Ocr.Google/G1ANT.Addon.Ocr.Google/Variables/TimeoutOcrVariable.md) | Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
| `errorcall`    | [procedure](G1ANT.Language/G1ANT.Language/Structures/ProcedureStructure.md) | no       |                                                              | Name of a procedure to call when the command throws an exception or when a given `timeout` expires |
| `errorjump`    | [label](G1ANT.Language/G1ANT.Language/Structures/LabelStructure.md) | no       |                                                              | Name of the label to jump to when the command throws an exception or when a given `timeout` expires |
| `errormessage` | [text](G1ANT.Language/G1ANT.Language/Structures/TextStructure.md) | no       |                                                              | A message that will be shown in case the command throws an exception or when a given `timeout` expires, and no `errorjump` argument is specified |
| `errorresult`  | [variable](G1ANT.Language/G1ANT.Language/Structures/VariableStructure.md) | no       |                                                              | Name of a variable that will store the returned exception. The variable will be of [error](G1ANT.Language/G1ANT.Language/Structures/ErrorStructure.md) structure |

For more information about `if`, `timeout`, `errorcall`, `errorjump`, `errormessage` and `errorresult` arguments, see [Common Arguments](G1ANT.Manual/appendices/common-arguments.md) page.

## Example

In this example the robot opens an image in Chrome, searches for a word “alice” in this image and displays its coordinates in a dialog box:

```G1ANT
♥googleLogin = Provide your Google Cloud credential here
chrome http://digitalnativestudios.com/textmeshpro/docs/rich-text/lowercase-uppercase-smallcaps.png
window ✱Chrome✱ style maximize
delay milliseconds 200
ocrgoogle.login ♥googleLogin
ocrgoogle.findpoint alice
dialog ♥result
```

> **Note:** In order to use any `ocrgoogle.` command, you have to be logged in to the Google Cloud Services with the [`ocrgoogle.login`](OcrGoogleLoginCommand.md) command.

