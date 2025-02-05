using DataLager;
using EntitetsLager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffärsLager
{
    public class UtrustningController
    {
        private readonly UnitOfWork _unitOfWork;
        public UtrustningController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void RegistreraUtrustning(string namn, string kategori, string skick, int tillgängliga)
        {
            Utrustning utrustning = new Utrustning()
            {
                Namn = namn,
                Kategori = kategori,
                Skick = skick,
                Tillgängliga = tillgängliga
            };

            _unitOfWork.UtrustningRepository.Add(utrustning);
            _unitOfWork.Save();
        }

        public void UppdateraUtrustning(int utrustningID, Utrustning utrustningattUppdatera)
        {
            var utrustning = _unitOfWork.UtrustningRepository.FirstOrDefault(m => m.UtrustningID == utrustningID);
            if (utrustning != null)
            {
                utrustning.Namn = utrustningattUppdatera.Namn;
                utrustning.Tillgängliga = utrustningattUppdatera.Tillgängliga;
                utrustning.Kategori = utrustningattUppdatera.Kategori;
                utrustning.Skick = utrustningattUppdatera.Skick;
                _unitOfWork.Save();
            }
        }

        public void TaBortUtrustning(int utrustningID)
        {
            var utrustning = _unitOfWork.UtrustningRepository.FirstOrDefault(m => m.UtrustningID == utrustningID);

            if (utrustning != null)
            {
                _unitOfWork.UtrustningRepository.Remove(utrustning);
                _unitOfWork.Save();
            }
        }

        public bool VisaUtrustningStatus(int utrustningID)
        {
            var utrustning = _unitOfWork.UtrustningRepository.FirstOrDefault(u => u.UtrustningID == utrustningID);
            return utrustning != null && utrustning.Tillgängliga > 0;
        }

        public List<string> HämtaKategorier()
        {
            return new List<string> { "Racketar", "Bollar", "Klubbor", "Mål" };
        }

        public List<string> HämtaSkick()
        {
            return new List<string> { "Ny", "God", "Sliten", "Trasig" };
        }

        public IEnumerable<Utrustning> HämtaAllUtrustning()
        {
            return _unitOfWork.UtrustningRepository.GetAll();
        }

        public IEnumerable<Utrustning> HämtaTillgängligUtrustning()
        {
            return _unitOfWork.UtrustningRepository.GetAll().Where(u => u.Tillgängliga > 0);
        }
    }
}
