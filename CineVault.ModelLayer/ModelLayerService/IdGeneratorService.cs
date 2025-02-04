using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.ModelLayer.ModelLayerService
{
    // Zal niet meer gebruikt worden, omdat deze klasse diende als test, nog voor het ID werd gegenereerd in de databank.
    /// Deze klasse zal dienen om de unieke ID-nummers te creëren voor elke gecreëerde persoon.
    /// Dit is omdat de setter privé is in elke klasse.
    /// Deze klasse zal dienen om de unieke ID-nummers te creëren voor elke gecreëerde persoon. 

    public static class IdGeneratorService
    {
        private static int _intCurrentId = 1;

        public static int GenerateId()
        {
            return _intCurrentId++;   
        }

        public static void Reset() 
        { 
            _intCurrentId = 1; 
        }

    }

}
