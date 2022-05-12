using System.Collections.Generic;

namespace Api.Models
{
    public class SubcategoriaMetadadoModel
    {
        public List<SubcategoriaModel> resultado { get; set; }

        #region  Metadado
        public MetadadoRetorno metadado { get; set; }
        #endregion

    }
}