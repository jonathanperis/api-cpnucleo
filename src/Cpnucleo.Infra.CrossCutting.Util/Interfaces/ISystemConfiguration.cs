﻿namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    public interface ISystemConfiguration
    {
        /// <summary>
        /// Connection to the Azure Database
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// JWT key for token auth
        /// </summary>
        string JwtKey { get; }

        /// <summary>
        /// JWT issuer
        /// </summary>
        string JwtIssuer { get; }

        /// <summary>
        /// JWT token expiration date
        /// </summary>
        int JwtExpires { get; }
    }
}
