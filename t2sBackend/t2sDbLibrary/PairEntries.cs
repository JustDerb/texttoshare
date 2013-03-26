﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sDbLibrary
{
    public static class PairEntries
    {
        /// <summary>
        /// Used for determining the maximum number of failed attempts a plugin can have before it is disabled globally.
        /// </summary>
        public static readonly string PLUGIN_THRESHOLD = "plugin_threshold";
    }
}
