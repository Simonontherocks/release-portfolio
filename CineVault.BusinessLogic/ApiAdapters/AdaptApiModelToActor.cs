using CineVault.BusinessLogic.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.ApiAdapters
{
    public class AdaptApiModelToActor
    {
        private Cast _apiActor;
        private ModelLayer.ModelMovie.Actor  _neadedActor;

        public AdaptApiModelToActor(Cast apiActor, ModelLayer.ModelMovie.Actor neadedActor)
        {
            _apiActor = apiActor;
            _neadedActor = neadedActor;
            ConvertToOriginal(_apiActor, neadedActor);
        }

        private void ConvertToOriginal(Cast apiDirector, ModelLayer.ModelMovie.Actor neadedDirector)
        {
            neadedDirector.Name = apiDirector.Name;
        }
    }
}
