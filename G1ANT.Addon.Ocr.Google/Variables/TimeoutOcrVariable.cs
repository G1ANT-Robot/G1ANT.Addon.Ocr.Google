using G1ANT.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1ANT.Addon.Ocr.Google.Commands
{
    [Variable(
        Name = "timeoutocr",
        Tooltip = "Determines the timeout value (in ms) for the `ocrgoogle.` and the `ocrtesseract.` commands; the default value is 60000 (60 seconds)")]
    public class TimeoutOcrVariable : Variable
    {
        private TimeSpanStructure value;

        public TimeoutOcrVariable(AbstractScripter scripter = null) : base(scripter)
        {
            value = new TimeSpanStructure(new TimeSpan(0, 0, 60), "", scripter);
        }

        public override Structure GetValue(string index = null)
        {
            return value.Get(index);
        }

        public override void SetValue(Structure value, string index = null)
        {
            this.value.Set(value, index);
        }
    }
}
