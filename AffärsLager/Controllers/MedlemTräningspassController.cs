using DataLager;
using EntitetsLager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffärsLager
{
    public class MedlemTräningspassController
    {
        private readonly UnitOfWork _unitOfWork;

        public MedlemTräningspassController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void LäggTillMedlempåTräningspass(int medlemID, int passID)
        {
            var medlem = _unitOfWork.MedlemRepository.FirstOrDefault(m => m.MedlemID == medlemID);
            var träningspass = _unitOfWork.TräningspassRepository.FirstOrDefault(t => t.TräningspassID == passID);

            if (medlem != null && träningspass != null)
            {
                var medlemTräningspass = _unitOfWork.MedlemTräningspassRepository
                    .FirstOrDefault(mt => mt.MedlemID == medlemID && mt.TräningspassID == passID);

                if (medlemTräningspass != null)
                {
                    throw new InvalidOperationException("Medlemmen är redan anmäld till detta träningspass.");
                }

                var nyttMedlemTräningspass = new MedlemTräningspass
                {
                    MedlemID = medlemID,
                    TräningspassID = passID
                };

                _unitOfWork.MedlemTräningspassRepository.Add(nyttMedlemTräningspass);
                _unitOfWork.Save();
            }
        }

        public void TaBortMedlemFrånPass(int medlemID, int passID)
        {
            var medlemTräningspass = _unitOfWork.MedlemTräningspassRepository
                .FirstOrDefault(mt => mt.MedlemID == medlemID && mt.TräningspassID == passID);

            if (medlemTräningspass != null)
            {
                _unitOfWork.MedlemTräningspassRepository.Remove(medlemTräningspass);
                _unitOfWork.Save();
            }
            else
            {
                throw new InvalidOperationException("Medlemmen är inte anmäld till detta träningspass.");
            }
        }

        public List<Medlem> HämtaDeltagareFörTräningspass(int träningspassID)
        {
            var medlemTräningspassList = _unitOfWork.MedlemTräningspassRepository
                .GetAll()
                .Where(mt => mt.TräningspassID == träningspassID)
                .ToList();

            var deltagare = medlemTräningspassList
                .Select(mt => mt.Medlem)
                .ToList();

            return deltagare;
        }
    }
}
