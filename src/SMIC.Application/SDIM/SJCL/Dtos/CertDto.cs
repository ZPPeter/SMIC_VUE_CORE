using SMIC.SJCL;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMIC.SDIM.SJCL.Dtos
{
    public class CertDto
    {
        public int[] Signer { get; set; }
        public JDJLFM jdjlfm;
        public RawTemplate rawTemplate;
    }
}
