using System.Collections.Generic;

namespace Api.Models
{
    public class LancamentoMetadadoModel
    {
        public List<LancamentoModel> resultado { get; set; }

        #region  Metadado
        public MetadadoRetorno metadado { get; set; }
        #endregion

    }
}