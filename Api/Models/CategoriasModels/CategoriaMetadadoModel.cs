using System.Collections.Generic;

namespace Api.Models
{
    public class CategoriaMetadadoModel
    {
        public List<CategoriaModel> resultado { get; set; }

        #region  Metadado
        public MetadadoRetorno metadado { get; set; }
        #endregion

    }
}