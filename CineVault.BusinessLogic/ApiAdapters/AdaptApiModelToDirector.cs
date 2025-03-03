using CineVault.BusinessLogic.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.ApiAdapters
{
    public class AdaptApiModelToDirector
    {
        private Crew _apiDirector;
        private ModelLayer.ModelMovie.Director _neededDirector;

        public AdaptApiModelToDirector(Crew apiDirector, ModelLayer.ModelMovie.Director neededDirector)
        {
            _apiDirector = apiDirector;
            _neededDirector = neededDirector;
            ConvertToOriginal(_apiDirector, _neededDirector);
        }

        private void ConvertToOriginal(Crew apiDirector, ModelLayer.ModelMovie.Director neededDirector)
        {
            neededDirector.Name = apiDirector.Name;
        }
    }

}
