using System;
using System.Collections.Generic;
using infrastructure.DatabaseManager.Interface;
using infrastructure.Entities.Helper;
using infrastructure.Repositories.Interface;

namespace infrastructure.Repositories.Factory
{
    public class CRUDFactory
    {
        private Dictionary<RepoType, object> repositories;
        private IDBConnection dbConnection;

        public CRUDFactory(IDBConnection dbConnection)
        {
            repositories = new Dictionary<RepoType, object>();
            this.dbConnection = dbConnection;
        }

        public ICrud<T> GetRepository<T>(RepoType repoType)
        {
            if (!repositories.ContainsKey(repoType))
            {
                object repoInstance;
                switch (repoType)
                {
                    case RepoType.ProductRepo:
                        repoInstance = new ProductRepository(dbConnection);
                        break;
                    case RepoType.UserRepo:
                        repoInstance = new UserRepository(dbConnection);
                        break;
                    case RepoType.ColorMapperRepo:
                        repoInstance = new ColorMapperRepository(dbConnection);
                        break;
                    case RepoType.ColorTypeRepo:
                        repoInstance = new ColorTypeRepository(dbConnection);
                        break;
                    case RepoType.SizeMapperRepo:
                        repoInstance = new SizeMapperRepository(dbConnection);
                        break;
                    case RepoType.SizeTypeRepo:
                        repoInstance = new SizeTypeRepository(dbConnection);
                        break;
                    case RepoType.UserProdRepo:
                        repoInstance = new UserProdRepository(dbConnection);
                        break;
                    case RepoType.LoginRepo:
                        repoInstance = new LoginRepository(dbConnection);
                        break;
                    default:
                        throw new ArgumentException("Invalid repository type");
                }
                repositories[repoType] = repoInstance;
            }
            return (ICrud<T>)repositories[repoType];
        }   
    }
}