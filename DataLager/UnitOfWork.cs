using EntitetsLager;

namespace DataLager
{
    /// <summary>
    ///  This class is used to access the storage in the application.
    /// </summary>
    public class UnitOfWork
    {
        private readonly ApplikationDbContext _context;

        public Repository<Medlem> MedlemRepository { get; set; }
        public Repository<Utrustning> UtrustningRepository { get; set; }
        public Repository<Träningspass> TräningspassRepository { get; set; }
        public Repository<Tränare> TränareRepository { get; set; }
        public Repository<Utlåning> UtlåningRepository { get; set; }
        public Repository<MedlemTräningspass> MedlemTräningspassRepository { get; set; }

        /// <summary>
        ///  Create a new instance.
        /// </summary>
        public UnitOfWork(ApplikationDbContext context)
        {
            _context = context;
            MedlemRepository = new Repository<Medlem>(_context);
            UtrustningRepository = new Repository<Utrustning>(_context);
            TränareRepository = new Repository<Tränare>(_context);
            TräningspassRepository = new Repository<Träningspass>(_context);
            UtlåningRepository = new Repository<Utlåning>(_context);
            MedlemTräningspassRepository = new Repository<MedlemTräningspass>(_context);


            // Initialize the tables if this is the first UnitOfWork.
            if (UtlåningRepository.IsEmpty())
            {
                Fill();
            }
        }

        /// <summary>
        ///  Save the changes made. Does nothing in this case.
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Fill()
        {

            if (!MedlemRepository.IsEmpty())
                return;

            MedlemRepository.Add(new Medlem
            {
                Namn = "Oscar Karlsson",
                Telefonnummer = "0701234567",
                Födelse = new DateTime(1985, 5, 2),
                Epost = "Oscar.Karlsson@gmail.com",
                Betalstatus = false
            });

            MedlemRepository.Add(new Medlem
            {
                Namn = "Victor Berg",
                Telefonnummer = "0707654321",
                Födelse = new DateTime(1982, 10, 22),
                Epost = "Victor.Berg@gmail.com",
                Betalstatus = true
            });

         
            //TRÄNARE
            TränareRepository.Add(new Tränare
            {
                Namn = "Ramtin Rahimi",
                Specialisering = "Tennis",
                Lösenord = "tennis123"
            });

            TränareRepository.Add(new Tränare
            {
                Namn = "Emil Torkildsen",
                Specialisering = "Paddel",
                Lösenord = "paddel456"
            });

            Save();
            //TRÄNINGSPASS
            TräningspassRepository.Add(new Träningspass
            {
                Aktivitet = "Tennis",
                Datum = new DateTime(2025, 2, 15),
                Tid = new TimeSpan(10, 0, 0),
                Plats = "Tennisplan A",
                TränareID = 1
            });

            
            TräningspassRepository.Add(new Träningspass
            {
                Aktivitet = "Paddel",
                Datum = new DateTime(2025, 2, 16),
                Tid = new TimeSpan(12, 0, 0),
                Plats = "Paddelsal B",
                TränareID = 2
            });

            Save();
            //UTRUSTNING
            UtrustningRepository.Add(new Utrustning
            {
                Namn = "Tennis-racket",
                Kategori = "Racketar",
                Skick = "Ny",
                Tillgängliga = 10
            });

            UtrustningRepository.Add(new Utrustning
            {
                Namn = "Paddel-racket",
                Kategori = "Racketar",
                Skick = "God",
                Tillgängliga = 15
            });

            UtrustningRepository.Add(new Utrustning
            {
                Namn = "Tennis-boll",
                Kategori = "Bollar",
                Skick = "Sliten",
                Tillgängliga = 20
            });

            UtrustningRepository.Add(new Utrustning
            {
                Namn = "Paddel-boll",
                Kategori = "Bollar",
                Skick = "God",
                Tillgängliga = 30
            });

            Save();
            //UTLÅNING
            UtlåningRepository.Add(new Utlåning
            {
                MedlemID = 1,
                UtrustningID = 2,
                UtLåningsdatum = new DateTime(2025, 2, 1),
                Återlämningsdatum = new DateTime(2025, 2, 8)
            });

            UtlåningRepository.Add(new Utlåning
            {
                MedlemID = 2,
                UtrustningID = 1,
                UtLåningsdatum = new DateTime(2025, 2, 3),
                Återlämningsdatum = new DateTime(2025, 2, 10)
            });

            Save();
            //MEDLEMTRÄNINGSPASS
            MedlemTräningspassRepository.Add(new MedlemTräningspass
            {
                MedlemID = 2,
                TräningspassID = 1
            });

            MedlemTräningspassRepository.Add(new MedlemTräningspass
            {
                MedlemID = 1,
                TräningspassID = 2
            });

            Save();
        }
    }
}