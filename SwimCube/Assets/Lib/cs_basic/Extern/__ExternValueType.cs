using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Giu.Basic {
    public static class __ExternValueType {

        public static int ToInt(this float f, int multiple = 1) { return ((int)(f * multiple)); }
    }
}
