using AdminLibrary.Models.Shared;

namespace AdminLibrary.Models.Entities
{
    public class MaterialsModel : EntityBase<int>
    {
        public MaterialsModel()
        {
            
        }
        public MaterialsModel(
            string identifier, 
            string title, 
            DateTime registerDate,
            int registerQuantity,
            int currentQuantity
            )
        {
            Identifier = identifier; 
            Title = title;
            RegisterDate = registerDate;
            RegisterQuantity = registerQuantity;
            CurrentQuantity = currentQuantity;
        }
        public string Identifier { get; set; }
        public string Title { get; set; }
        public DateTime RegisterDate { get; set; }
        public int RegisterQuantity { get; set; } 
        public int CurrentQuantity { get; set; }

        public void Cast(ref MaterialsModel model)
        {
            model.Identifier = Identifier;
            model.Title = Title;
            model.RegisterDate = RegisterDate;
            model.RegisterQuantity = RegisterQuantity;
            model.CurrentQuantity = CurrentQuantity;

        }
    }

   
}
