using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLager;
using EntitetsLager;

namespace AffärsLager
{
    public class MedlemController
    {
        private readonly UnitOfWork _unitOfWork;

        public MedlemController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string LäggTillMedlem(string namn, DateTime födelse, string telefonnummer, string epost)
        {
            if (!ÄrMedlemUnik(namn, telefonnummer, epost))
            {
                return "Det finns redan en medlem med samma namn, telefonnummer eller e-post.";
            }


            Medlem medlem = new Medlem
            {
                Namn = namn,
                Födelse = födelse,
                Telefonnummer = telefonnummer,
                Epost = epost,
                Betalstatus = false
            };

            _unitOfWork.MedlemRepository.Add(medlem);
            _unitOfWork.Save();

            return "Medlem har lagts till!";

        }
        public void UppdateraMedlem(int medlemID, Medlem medlemattUppdatera)
        {
            var medlem = _unitOfWork.MedlemRepository.FirstOrDefault(m => m.MedlemID == medlemID);
            if (medlem != null)
            {
                medlem.Namn = medlemattUppdatera.Namn;
                medlem.Telefonnummer = medlemattUppdatera.Telefonnummer;
                medlem.Födelse = medlemattUppdatera.Födelse;
                medlem.Epost = medlemattUppdatera.Epost;
                medlem.Betalstatus = medlemattUppdatera.Betalstatus;

                _unitOfWork.Save();
            }
        }
        public void TaBortMedlem(int medlemID)
        {
            var medlem = _unitOfWork.MedlemRepository.FirstOrDefault(m => m.MedlemID == medlemID);
            if (medlem != null)
            {
                _unitOfWork.MedlemRepository.Remove(medlem);
                _unitOfWork.Save();
            }
        }
        public bool VisaMedlemStatus(int medlemID)
        {
            var medlem = _unitOfWork.MedlemRepository.FirstOrDefault(m => m.MedlemID == medlemID);
            if (medlem != null)
            {
                return medlem.Betalstatus;
            }
            return false;
        }

        public bool ÄrMedlemUnik(string namn, string telefonnummer, string epost)
        {
            var medlem = _unitOfWork.MedlemRepository.FirstOrDefault(m => m.Namn == namn || m.Telefonnummer == telefonnummer || m.Epost == epost);

            return medlem == null;
        }

        public IEnumerable<Medlem> HämtaAllaMedlemmar()
        {
            return _unitOfWork.MedlemRepository.GetAll();
        }
    }
}
