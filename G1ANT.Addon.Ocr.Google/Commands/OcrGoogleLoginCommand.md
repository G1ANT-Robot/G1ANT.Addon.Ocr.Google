# ocrgoogle.login

## Syntax

```G1ANT
ocrgoogle.login jsoncredential ⟦text⟧
```

## Description

This command logs in to the Google Cloud text recognition service.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`jsoncredential`| [text](G1ANT.Language/G1ANT.Language/Structures/TextStructure.md) | yes |  | JSON credential obtained from the Google Cloud text recognition service |
| `if`           | [bool](G1ANT.Language/G1ANT.Language/Structures/BooleanStructure.md) | no       | true                                                        | Executes the command only if a specified condition is true   |
| `timeout`      | [timespan](G1ANT.Language/G1ANT.Language/Structures/TimeSpanStructure.md) | no       | [♥timeoutcommand](G1ANT.Language/G1ANT.Addon.Core/Variables/TimeoutCommandVariable.md) | Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
| `errorcall`    | [procedure](G1ANT.Language/G1ANT.Language/Structures/ProcedureStructure.md) | no       |                                                             | Name of a procedure to call when the command throws an exception or when a given `timeout` expires |
| `errorjump`    | [label](G1ANT.Language/G1ANT.Language/Structures/LabelStructure.md) | no       |                                                             | Name of the label to jump to when the command throws an exception or when a given `timeout` expires |
| `errormessage` | [text](G1ANT.Language/G1ANT.Language/Structures/TextStructure.md) | no       |                                                             | A message that will be shown in case the command throws an exception or when a given `timeout` expires, and no `errorjump` argument is specified |
| `errorresult`  | [variable](G1ANT.Language/G1ANT.Language/Structures/VariableStructure.md) | no       |                                                             | Name of a variable that will store the returned exception. The variable will be of [error](G1ANT.Language/G1ANT.Language/Structures/ErrorStructure.md) structure  |

For more information about `if`, `timeout`, `errorcall`, `errorjump`, `errormessage` and `errorresult` arguments, see [Common Arguments](G1ANT.Manual/appendices/common-arguments.md) page.

### Obtaining Google Cloud Platform credentials

Visit [Google Cloud Vision](https://cloud.google.com/vision/) website and apply for a trial account. You will then receive a JSON credential necessary to use the `ocrgoogle.` commands.

## Example

In this example the robot opens DuckDuckGo website in Chrome and searches for “duckduckgo” text. But before any text recognition can be performed, the robot needs to log in to Google Cloud Platform with a credential provided in the `♥googleLogin` variable:

```G1ANT
♥googleLogin = Provide your Google Cloud credential here
chrome duckduckgo.com
ocrgoogle.login ♥googleLogin
ocrgoogle.find duckduckgo
dialog ♥result
```

