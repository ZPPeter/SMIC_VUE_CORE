using System;
using System.Collections.Generic;
using System.Text;

namespace SMIC.SJCL
{
    public abstract class BasePluginsService
    { 
        // public abstract string Handle(RawTemplate template, JDJLFM jdjlfm, int[] Signer);
        public abstract string[] Handle(RawTemplate template, JDJLFM jdjlfm, int[] Signer);        
    }
}
