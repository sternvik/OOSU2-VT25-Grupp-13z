using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitetsLager;
using DataLager;
using System.Net.Http.Headers;

namespace AffärsLager
{
    public class TränareController
    {
        private readonly UnitOfWork _unitOfWork;

        public TränareController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void LäggTillTränare(Tränare tränare)
        {
            _unitOfWork.TränareRepository.Add(tränare);
            _unitOfWork.Save();
        }

        public void UppdateraTränare(int tränareID, Tränare tränareattUppdatera)
        {
            var tränare = _unitOfWork.TränareRepository.FirstOrDefault(m => m.TränareID == tränareID);
            if (tränare != null)
            {
                tränare.Namn = tränareattUppdatera.Namn;
                tränare.Specialisering = tränareattUppdatera.Specialisering;
                tränare.Lösenord = tränareattUppdatera.Lösenord;

                _unitOfWork.Save();
            }
        }

        public void TaBortTränare(int tränareID)
        {
            var tränare = _unitOfWork.TränareRepository.FirstOrDefault(m => m.TränareID == tränareID);
            if (tränare != null)
            {
                _unitOfWork.TränareRepository.Remove(tränare);
                _unitOfWork.Save();
            }
        }

        public Tränare Visatränaredetaljer(int tränareID)
        {
            var tränare = _unitOfWork.TränareRepository.FirstOrDefault(m => m.TränareID == tränareID);
            return tränare;
        }

        public List<string> HämtaSpecialisering()
        {
            return new List<string> { "Paddel", "Tennis", "Pingis", "Squash", "Badminton", "Innebandy" };
        }

        public IEnumerable<Tränare> HämtaAllaTränare()
        {
            return _unitOfWork.TränareRepository.GetAll();
        }
    }
}
