using Microsoft.EntityFrameworkCore;
using OnlineStore.DAO.Model;
using OnlineStore.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DAO
{
    public class DAOEntity_Framfork
    {
        DAOContext dAOContext = null;

        public DAOEntity_Framfork()
        {
            dAOContext = new DAOContext();
        }

        private void init()
        {
            dAOContext.shope.Load();
            dAOContext.photo.Load();
            dAOContext.dataTime.Load();
            dAOContext.shopeTb.Load();
        }

        public bool CheckNameShopeInDb(string nameShope)
        {
            bool isShopeDB = default;
            init();
            try
            {
                if (nameShope == null)
                    throw new ArgumentNullException("Object reference is not specified");

                ModelAllShoppe modelAllShoppe =  dAOContext.shope.FirstOrDefault(sh => sh.nameShope == nameShope);
                if (modelAllShoppe != null)
                {
                    isShopeDB = true;
                }
                else
                {
                    isShopeDB = false;
                }
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentNullException(e.Message);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            
            return isShopeDB;
        }

        public async Task AddAllProductInDB(string nameShope)
        {
            init();
            ModelAllShoppe modelAllShoppe = SetModelAllShope();
            modelAllShoppe.nameShope      = nameShope;
            await dAOContext.shope.AddAsync(modelAllShoppe);
            await dAOContext.SaveChangesAsync();
        }

        private ModelAllShoppe SetModelAllShope()
        {
            ModelAllShoppe modelAllShoppe = null;
            ModelProductDAO modelProduct  = null;
            ModelPhoto modelPhoto         = null;
            ModelDatePrice modelDate      = null;
            modelAllShoppe                = new ModelAllShoppe();
            modelAllShoppe.shope          = new List<ModelProductDAO>();
            
            foreach (var product in ManagerShope.listProduct)
            {
                modelProduct                = new ModelProductDAO();
                modelPhoto                  = new ModelPhoto();
                modelDate                   = new ModelDatePrice();
                modelProduct.listPhoto      = new List<ModelPhoto>();
                modelProduct.ModelDatePrice = new List<ModelDatePrice>();

                modelDate.dataTime          = DateTime.Today.ToLongDateString();
                modelDate.price             = product.price;
                modelProduct.listPhoto.AddRange(DeserizerPhotoInStr(product.listPhoto));
                modelProduct.ModelDatePrice.Add(modelDate);
                modelProduct.description    = product.description;
                modelProduct.id             = product.id;
                modelProduct.nameProduct    = product.nameProduct;
                modelAllShoppe.shope.Add(modelProduct);
            }
            return modelAllShoppe;
        }
        
        private List<ModelPhoto> DeserizerPhotoInStr(List<string> listPhoto)
        {
            ModelPhoto modelPhoto       = null;
            List<ModelPhoto> mListPhoto = new List<ModelPhoto>();
            foreach(var photo in listPhoto)
            {
                modelPhoto       = new ModelPhoto();
                modelPhoto.photo = photo;
                mListPhoto.Add(modelPhoto);
            }
            return mListPhoto;
        }

        public bool CheckInDbOnDate(string nameShope, string currentData)
        {
            bool isData = default;
            init();
            try
            {
                if (nameShope == null || currentData == null)
                    throw new ArgumentNullException("Object reference is not specified");
                
                var curentProduct = dAOContext.shope.FirstOrDefault(s => s.nameShope == nameShope);
                if (curentProduct == null)
                    throw new ArgumentNullException("Object reference is not specified");

                var curentData = curentProduct.shope
                    .Select(p => p.ModelDatePrice
                    .FirstOrDefault(d => d.dataTime == currentData) != null)
                    .Contains(false);
                if (!curentData)
                {
                    isData = true;
                }
                else
                {
                    isData = false;
                }
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentNullException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return isData;
        }

        public List<ModelProductDAO> GetAllProduct(string nameShope, bool isShopeDB)
        {
            init();
            ModelAllShoppe listCurentShope = null;
            try
            {
                if (nameShope == null)
                    throw new ArgumentNullException("Object reference is not specified");

                listCurentShope = dAOContext.shope.FirstOrDefault(s => s.nameShope == nameShope);
                if (listCurentShope == null)
                    throw new ArgumentNullException("Object reference is not specified");

                if (isShopeDB)
                {
                    string price = null;
                    string nameProduct = null;
                    string description = null;
                    List<string> listPhoto = null;
                    string id = null;
                    Product product = null;
                    string tempDate = DateTime.Today.ToLongDateString();

                    foreach (var prod in listCurentShope.shope)
                    {
                        listPhoto = new List<string>();
                        price = prod.ModelDatePrice.Last().price;
                        nameProduct = prod.nameProduct;
                        description = prod.description;
                        id = prod.id;
                        foreach (var photo in prod.listPhoto)
                        {
                            listPhoto.Add(photo.photo);
                        }
                        product = new Product(price, description, listPhoto, nameProduct, id);
                        product.dataTime = prod.ModelDatePrice;
                        ManagerShope.listProduct.Add(product);
                    }
                    return null;
                }
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentNullException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listCurentShope.shope;
        }

        public  void UpdateProduct(string nameShopr)
        {
            
            init();
            Remove(nameShopr);
            init();
            Add(nameShopr);
            init();

            ModelDatePrice price = null;
            var shope = dAOContext.shope.ToList().FirstOrDefault(s => s.nameShope == nameShopr).shope;
            foreach (var prpod in ManagerShope.listProduct)
            {
                try
                {
                    price = new ModelDatePrice();
                    string tempDataPrice     = DateTime.Today.ToLongDateString();
                    var product              = shope.FirstOrDefault(p => p.id == prpod.id);
                    ModelDatePrice dataPrice = product.ModelDatePrice.FirstOrDefault(d => d.dataTime == "");
                    if (dataPrice == null)
                    {
                        price.dataTime = DateTime.Today.ToLongDateString();
                        price.id       = prpod.id;
                        price.price    = prpod.price;
                        dAOContext.dataTime.Add(price);
                        dAOContext.SaveChanges();
                    }
                }
                catch(Exception) { }
            }
            
        }

        private void Remove(string nameShopr)
        {
            List<ModelProductDAO> modelProductDAOs = new List<ModelProductDAO>();
            ModelProductDAO modelProductDAO        = null;
            var prod                               = dAOContext.shope.FirstOrDefault(s => s.nameShope == nameShopr).shope;
            foreach (var product in prod)
            {
                modelProductDAO = new ModelProductDAO();
                var s           = ManagerShope.listProduct.Find(p => p.id == product.id);
                if (s == null)
                {
                    modelProductDAOs.Add(product);
                }
            }
            if (modelProductDAOs.Count != 0)
            {
                dAOContext.shopeTb.RemoveRange(modelProductDAOs);
                dAOContext.SaveChanges();
            }
        }

        private void Add(string nameShopr)
        {
            List<ModelProductDAO> modelProductDAOs   = new List<ModelProductDAO>();
            ModelProductDAO modelProductDAO          = new ModelProductDAO();
            List<ModelDatePrice> listmodelDatePrices = null;
            ModelDatePrice modelDatePrice            = null;
            var prod = dAOContext.shope.FirstOrDefault(s => s.nameShope == nameShopr);
            foreach (var product in ManagerShope.listProduct)
            {
                modelDatePrice      = new ModelDatePrice();
                listmodelDatePrices = new List<ModelDatePrice>();
                var s = prod.shope.FirstOrDefault(p => p.id == product.id);
                if (s == null)
                {
                    modelDatePrice.dataTime        = DateTime.Today.ToLongDateString();
                    modelDatePrice.price           = product.price;
                    listmodelDatePrices.Add(modelDatePrice);
                    modelProductDAO.description = product.description;
                    modelProductDAO.listPhoto      = DeserizerPhotoInStr(product.listPhoto);
                    modelProductDAO.ModelDatePrice = listmodelDatePrices;
                    modelProductDAO.nameProduct    = product.nameProduct;
                    modelProductDAO.id             = product.id;
                    dAOContext.shope.Update(prod);
                    dAOContext.SaveChanges();
                }
                
            }
            
        }
    }
}
