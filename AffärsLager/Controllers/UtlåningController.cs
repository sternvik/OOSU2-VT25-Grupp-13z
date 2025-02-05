using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitetsLager;
using DataLager;

namespace AffärsLager
{
    public class UtlåningController
    {
        private readonly UnitOfWork _unitOfWork;
        public UtlåningController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void RegistreraUtlåning(int medlemID, int utrustningID, DateTime utlåningsdatum, DateTime? återlämningsdatum)
        {
            var medlem = _unitOfWork.MedlemRepository.FirstOrDefault(p => p.MedlemID == medlemID);
            var utrustning = _unitOfWork.UtrustningRepository.FirstOrDefault(p => p.UtrustningID == utrustningID);

            if (medlem == null)
            {
                throw new InvalidOperationException($"Fel: Medlem med ID {medlemID} hittades inte.");
            }

            if (utrustning == null)
            {
                throw new InvalidOperationException($"Fel: Utrustning med inventarienummer {utrustningID} hittades inte.");
            }

            if (utrustning.Tillgängliga <= 0)
            {
                throw new InvalidOperationException($"Fel: Utrustning med inventarienummer {utrustningID} är inte tillgänglig.");
            }

            if (återlämningsdatum.HasValue && utlåningsdatum > återlämningsdatum.Value)
            {
                throw new InvalidOperationException("Fel: Utlåningsdatum kan inte vara senare än återlämningsdatum.");
            }

            var utlåning = new Utlåning
            {
                MedlemID = medlemID,
                UtrustningID = utrustningID,
                UtLåningsdatum = utlåningsdatum,
                Återlämningsdatum = återlämningsdatum
            };

            _unitOfWork.UtlåningRepository.Add(utlåning);
            utrustning.Tillgängliga--;

            if (återlämningsdatum.HasValue)
            {
                utrustning.Tillgängliga++;
            }

            _unitOfWork.Save();
        }


        public void RegistreraÅterlämning(int medlemID, int utrustningID, DateTime återlämningsdatum)
        {
            var utlåning = _unitOfWork.UtlåningRepository.FirstOrDefault(p =>
                    p.MedlemID == medlemID &&
                    p.UtrustningID == utrustningID &&
                    p.Återlämningsdatum == null);

            if (utlåning == null)
            {
                throw new InvalidOperationException("Fel: Ingen aktiv utlåning hittades för denna utrustning.");
            }

            if (återlämningsdatum < utlåning.UtLåningsdatum)
            {
                throw new InvalidOperationException("Fel: Återlämningsdatum kan inte vara före utlåningsdatum.");
            }

            utlåning.Återlämningsdatum = återlämningsdatum;

            var utrustning = _unitOfWork.UtrustningRepository.FirstOrDefault(p => p.UtrustningID == utrustningID);
            if (utrustning != null)
            {
                utrustning.Tillgängliga++;
                _unitOfWork.Save();
            }
        }

        public IEnumerable<Utlåning> HämtaAllaAktivaUtlåningar()
        {
            return _unitOfWork.UtlåningRepository.GetAll().Where(u => u.Återlämningsdatum == null || u.Återlämningsdatum > DateTime.Now);
        }

        public IEnumerable<Utlåning> HämtaArkiveradeUtlånignar()
        {
            return _unitOfWork.UtlåningRepository.GetAll().Where(u => u.Återlämningsdatum < DateTime.Now);
        }

        public IEnumerable<Utlåning> HämtaALlaUtlåningar()
        {
            return _unitOfWork.UtlåningRepository.GetAll();
        }
    }
}
