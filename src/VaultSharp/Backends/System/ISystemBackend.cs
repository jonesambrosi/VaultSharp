﻿using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.Backends.Auth;
using VaultSharp.Core;

namespace VaultSharp.Backends.System
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISystemBackend
    {
        /// <summary>
        /// Gets all the mounted audit backends (it does not list all available audit backends).
        /// </summary>
        /// <returns>
        /// The mounted audit backends.
        /// </returns>
        Task<Secret<Dictionary<string, AbstractAuditBackend>>> GetAuditBackendsAsync();

        /// <summary>
        /// Mounts a new audit backend at the specified mount point.
        /// </summary>
        /// <param name="abstractAuditBackend">The audit backend.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MountAuditBackendAsync(AbstractAuditBackend abstractAuditBackend);

        /// <summary>
        /// Unmounts the audit backend at the given mount point.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The mount point for the audit backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task UnmountAuditBackendAsync(string path);

        /// <summary>
        /// Hash the given input data with the specified audit backend's hash function and salt.
        /// This endpoint can be used to discover whether a given plaintext string (the input parameter) appears in
        /// the audit log in obfuscated form.
        /// Note that the audit log records requests and responses; since the Vault API is JSON-based,
        /// any binary data returned from an API call (such as a DER-format certificate) is base64-encoded by
        /// the Vault server in the response, and as a result such information should also be base64-encoded
        /// to supply into the <see cref="inputToHash" /> parameter.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The mount point for the audit backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="inputToHash"><para>[required]</para>
        /// The input value to hash</param>
        /// <returns>
        /// The hashed value.
        /// </returns>
        Task<Secret<AuditHash>> AuditHashAsync(string path, string inputToHash);

        /// <summary>
        /// Gets all the enabled authentication backends.
        /// </summary>
        /// <returns>
        /// The enabled authentication backends.
        /// </returns>
        Task<Secret<Dictionary<string, AuthBackend>>> GetAuthBackendsAsync();

        /// <summary>
        /// Mounts a new authentication backend.
        /// The auth backend can be accessed and configured via the auth path specified in the URL. 
        /// </summary>
        /// <param name="authBackend">The authentication backend.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MountAuthBackendAsync(AuthBackend authBackend);

        /// <summary>
        /// Unmounts the authentication backend at the given mount point.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The authentication path for the authentication backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task UnmountAuthBackendAsync(string path);

        /// <summary>
        /// Gets the mounted authentication backend's configuration values.
        /// The lease values for each TTL may be the system default ("0" or "system") or a mount-specific value.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The authentication path for the authentication backend in which to tune. 
        /// (with or without trailing slashes. it doesn't matter)</param>
        /// <returns>
        /// The mounted secret backend's configuration values.
        /// </returns>
        Task<Secret<BackendConfig>> GetAuthBackendConfigAsync(string path);

        /// <summary>
        /// Tunes the mount configuration parameters for the given <see cref="path" />.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The authentication path for the authentication backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="backendConfig"><para>[required]</para>
        /// The mount configuration with the required setting values.
        /// Provide a value of <value>"0"</value> for the TTL settings if you want to use the system defaults.</param>
        /// <returns>
        /// A task
        /// </returns>
        Task ConfigureAuthBackendAsync(string path, BackendConfig backendConfig);

        /// <summary>
        /// Gets the capabilities of the token on the given path.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// Path on which the token's capabilities will be checked.</param>
        /// <param name="token"><para>[required]</para>
        /// Token for which capabilities are being queried.</param>
        /// <returns>The list of capabilities.</returns>
        Task<Secret<TokenCapability>> GetTokenCapabilitiesAsync(string path, string token);

        /// <summary>
        /// Gets the capabilities of the token associated with the accessor on the given path.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// Path on which the token's capabilities will be checked.</param>
        /// <param name="tokenAccessor"><para>[required]</para>
        /// Accessor to the Token for which capabilities are being queried.</param>
        /// <returns>The list of capabilities.</returns>
        Task<Secret<TokenCapability>> GetTokenCapabilitiesByAcessorAsync(string path, string tokenAccessor);

        /// <summary>
        /// Gets the capabilities of the calling token.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// Path on which the token's capabilities will be checked.</param>
        /// <returns>The list of capabilities.</returns>
        Task<Secret<TokenCapability>> GetCallingTokenCapabilitiesAsync(string path);

        /// <summary>
        /// Gets the request headers configured to be audited.
        /// </summary>
        /// <returns></returns>
        Task<Secret<RequestHeaderSet>> GetAuditRequestHeadersAsync();

        /// <summary>
        /// Gets a particular request header.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <returns>Header details.</returns>
        Task<Secret<RequestHeader>> GetAuditRequestHeaderAsync(string name);

        /// <summary>
        /// Creates/updates the request header to be audited.
        /// </summary>
        /// <param name="name"><para>[required]</para>
        /// The name fo the header.
        /// </param>
        /// <param name="hmac"><para>[optional]</para>
        /// Specifies if this header's value should be HMAC'ed in the audit logs.
        /// </param>
        /// <returns>The task.</returns>
        Task PutAuditRequestHeaderAsync(string name, bool hmac = false);

        /// <summary>
        /// Deletes a particular request header.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <returns>Header details.</returns>
        Task DeleteAuditRequestHeaderAsync(string name);

        /// <summary>
        /// Gets the initialization status of Vault.
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <returns>
        /// The initialization status of Vault.
        /// </returns>
        Task<bool> GetInitStatusAsync();

        /// <summary>
        /// Initializes a new Vault. The Vault must not have been previously initialized. 
        /// The recovery options, as well as the stored shares option, are only available when using Vault HSM.
        /// </summary>
        /// <param name="initOptions"><para>[required]</para>
        /// The initialization options.
        /// </param>
        /// <returns>
        /// An object including the (possibly encrypted, if pgp_keys was provided) master keys and initial root token.
        /// </returns>
        Task<MasterCredentials> InitAsync(InitOptions initOptions);

        /// <summary>
        /// Seals the Vault. In HA mode, only an active node can be sealed. 
        /// Standby nodes should be restarted to get the same effect. 
        /// Requires a token with root policy or sudo capability on the path.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        Task SealAsync();

        /// <summary>
        /// Gets the seal status of the Vault.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <returns>
        /// The seal status of the Vault.
        /// </returns>
        Task<SealStatus> GetSealStatusAsync();

        /// <summary>
        /// Progresses the unsealing of the Vault.
        /// Enter a single master key share to progress the unsealing of the Vault.
        /// If the threshold number of master key shares is reached, Vault will attempt to unseal the Vault.
        /// Otherwise, this API must be called multiple times until that threshold is met.
        /// <para>
        /// Either the <see cref="masterShareKey" /> or <see cref="resetCompletely" /> parameter must be provided; 
        /// if both are provided, <see cref="resetCompletely" /> takes precedence.
        /// </para>
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <param name="masterShareKey">A single master share key.</param>
        /// <param name="resetCompletely">When <value>true</value>, the previously-provided unseal keys are discarded from memory 
        /// and the unseal process is completely reset.
        /// Default value is <value>false</value>.
        /// If you make a call with the value as <value>true</value>, it doesn't matter if this call has a valid unused <see cref="masterShareKey" />. 
        /// It'll be ignored.</param>
        /// <returns>
        /// The seal status of the Vault.
        /// </returns>
        Task<SealStatus> UnsealAsync(string masterShareKey = null, bool resetCompletely = false);

        /// <summary>
        /// Unseals the Vault in a single call.
        /// Provide all the master keys together.
        /// </summary>
        /// <param name="allMasterShareKeys">All the master share keys.</param>
        /// <returns>The final Seal Status after all the share keys are applied.</returns>
        Task<SealStatus> QuickUnsealAsync(string[] allMasterShareKeys);
    }
}
