using System;
using System.Linq;
using DataLager;
using EntitetsLager;
using System.Security.Cryptography;
using System.Text;


namespace AffärsLager
{
    public class SäkerhetsController
    {
        private readonly UnitOfWork _unitOfWork;
        public Tränare LoggedInUser { get; private set; }

        public SäkerhetsController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool LoggaIn(string användarnamn, string lösenord)
        {
            var tränare = _unitOfWork.TränareRepository.FirstOrDefault(t => t.Namn.Equals(användarnamn));

            if (tränare != null && tränare.Lösenord == lösenord)
            {
                LoggedInUser = tränare;
                return true;
            }

            return false;
        }

        public void SkapaTränare(string namn, string lösenord, string specialisering)
        {
            if (_unitOfWork.TränareRepository.FirstOrDefault(t => t.Namn.Equals(namn)) != null)
            {
                throw new Exception("En tränare med detta namn finns redan.");
            }

            var tränare = new Tränare
            {
                Namn = namn,
                Lösenord = lösenord,
                Specialisering = specialisering
            };

            _unitOfWork.TränareRepository.Add(tränare);
            _unitOfWork.Save();
        }
    }
}
