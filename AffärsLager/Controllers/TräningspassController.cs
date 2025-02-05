using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLager;
using EntitetsLager;

namespace AffärsLager
{
    public class TräningspassController
    {
        private readonly UnitOfWork _unitOfWork;
        public TräningspassController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool ÄrTränareTillgänglig(int tränareID, DateTime datum, TimeSpan tid)
        {
            var träningspass = _unitOfWork.TräningspassRepository.FirstOrDefault(t => t.TränareID == tränareID && t.Datum.Date == datum.Date && t.Tid == tid);

            return träningspass == null;
        }

        public bool ÄrPlatsTillgänglig(string plats, DateTime datum, TimeSpan tid)
        {
            var träningspass = _unitOfWork.TräningspassRepository.FirstOrDefault(t => t.Plats == plats && t.Datum.Date == datum.Date && t.Tid == tid);

            return träningspass == null;
        }

        public void SkapaTräningspass(string aktivitet, DateTime datum, TimeSpan tid, string plats, int tränareID)
        {

            var tränare = _unitOfWork.TränareRepository.FirstOrDefault(p => p.TränareID == tränareID);

            if (!ÄrTränareTillgänglig(tränareID, datum, tid))
            {
                throw new InvalidOperationException("Tränaren är redan bokad vid denna tid.");
            }

            if (!ÄrPlatsTillgänglig(plats, datum, tid))
            {
                throw new InvalidOperationException("Platsen är redan bokad vid denna tid.");
            }

            var träningspass = new Träningspass
            {
                Aktivitet = aktivitet,
                Datum = datum,
                Tid = tid,
                Plats = plats,
                TränareID = tränareID
            };

            _unitOfWork.TräningspassRepository.Add(träningspass);
            _unitOfWork.Save();

        }

        public void TaBortTräningspass(int träningspassID)
        {
            var träningspass = _unitOfWork.TräningspassRepository.FirstOrDefault(t => t.TräningspassID == träningspassID);
            if (träningspass != null)
            {
                _unitOfWork.TräningspassRepository.Remove(träningspass);
                _unitOfWork.Save();
            }

        }

        public void RedigeraTräningspass(int träningspassID, Träningspass träningspassattändra)
        {
            var träningspass = _unitOfWork.TräningspassRepository.FirstOrDefault(t => t.TräningspassID == träningspassID);
            if (träningspass != null)
            {
                träningspass.TränareID = träningspassattändra.TränareID;
                träningspass.Aktivitet = träningspassattändra.Aktivitet;
                träningspass.Datum = träningspassattändra.Datum;
                träningspass.Tid = träningspassattändra.Tid;
                träningspass.Plats = träningspassattändra.Plats;

                _unitOfWork.Save();
            }
        }

        public List<string> HämtaLokalerFörAktivitet(string aktivitet)
        {
            var lokaler = new List<string>();

            switch (aktivitet)
            {
                case "Paddel":
                    lokaler.Add("Paddelsal A");
                    lokaler.Add("Paddelsal B");
                    break;
                case "Tennis":
                    lokaler.Add("Tennisplan A");
                    lokaler.Add("Tennisplan B");
                    break;
                case "Pingis":
                    lokaler.Add("Pingisbord A");
                    lokaler.Add("Pingisbord B");
                    break;
                case "Squash":
                    lokaler.Add("Squashsal A");
                    lokaler.Add("Squashsal B");
                    break;
                case "Badminton":
                    lokaler.Add("Badmintonplan A");
                    lokaler.Add("Badmintonplan B");
                    break;
                case "Innebandy":
                    lokaler.Add("Innebandyplan A");
                    break;
            }

            return lokaler;
        }

        public List<string> HämtaTiderFörAktivitet()
        {
            List<string> tider = new List<string>();

            for (int i = 8; i < 21; i++)
            {
                string tid = $"{i}:00";
                tider.Add(tid);
            }

            return tider;
        }

        public IEnumerable<Träningspass> HämtaAllaTräningspass()
        {
            return _unitOfWork.TräningspassRepository.GetAll();
        }
    }
}
