namespace ClasificationProjects.Opi.Back.Domain.Entities
{
    public sealed class User
    {
        private User(
            System.Guid id,
            string tenantId,
            string externalId,
            System.Guid roleId,
            string email,
            bool isActive,
            System.DateTime createdAt,
            System.DateTime updatedAt,
            Role? role)
        {
            Id = id;
            TenantId = tenantId;
            ExternalId = externalId;
            RoleId = roleId;
            Email = email;
            IsActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Role = role;
        }

        public System.Guid Id { get; private set; }
        public string TenantId { get; private set; }
        public string ExternalId { get; private set; }
        public System.Guid RoleId { get; private set; }
        public string Email { get; private set; }
        public bool IsActive { get; private set; }
        public System.DateTime CreatedAt { get; private set; }
        public System.DateTime UpdatedAt { get; private set; }
        public Role? Role { get; private set; }

        public static User Create(string tenantId, string externalId, System.Guid roleId, string email, System.DateTime now)
        {
            if (string.IsNullOrWhiteSpace(tenantId))
                throw new System.ArgumentException(null, nameof(tenantId));

            if (tenantId.Length > 50)
                throw new System.ArgumentException(null, nameof(tenantId));

            if (externalId is null)
                throw new System.ArgumentException(null, nameof(externalId));

            if (externalId.Length > 50)
                throw new System.ArgumentException(null, nameof(externalId));

            if (roleId == System.Guid.Empty)
                throw new System.ArgumentException(null, nameof(roleId));

            if (string.IsNullOrWhiteSpace(email))
                throw new System.ArgumentException(null, nameof(email));

            if (email.Length > 200)
                throw new System.ArgumentException(null, nameof(email));

            return new User(
                System.Guid.NewGuid(),
                tenantId.Trim(),
                externalId.Trim(),
                roleId,
                email.Trim(),
                true,
                now,
                now,
                null
            );
        }

        public static User Rehydrate(
            System.Guid id,
            string tenantId,
            string externalId,
            System.Guid roleId,
            string email,
            bool isActive,
            System.DateTime createdAt,
            System.DateTime updatedAt,
            Role? role)
        {
            if (id == System.Guid.Empty)
                throw new System.ArgumentException(null, nameof(id));

            if (string.IsNullOrWhiteSpace(tenantId))
                throw new System.ArgumentException(null, nameof(tenantId));

            if (tenantId.Length > 50)
                throw new System.ArgumentException(null, nameof(tenantId));

            if (externalId is null)
                throw new System.ArgumentException(null, nameof(externalId));

            if (externalId.Length > 50)
                throw new System.ArgumentException(null, nameof(externalId));

            if (roleId == System.Guid.Empty)
                throw new System.ArgumentException(null, nameof(roleId));

            if (string.IsNullOrWhiteSpace(email))
                throw new System.ArgumentException(null, nameof(email));

            if (email.Length > 200)
                throw new System.ArgumentException(null, nameof(email));

            return new User(
                id,
                tenantId.Trim(),
                externalId.Trim(),
                roleId,
                email.Trim(),
                isActive,
                createdAt,
                updatedAt,
                role
            );
        }

        public void Update(System.Guid roleId, string email, bool isActive, System.DateTime now)
        {
            if (roleId == System.Guid.Empty)
                throw new System.ArgumentException(null, nameof(roleId));

            if (string.IsNullOrWhiteSpace(email))
                throw new System.ArgumentException(null, nameof(email));

            if (email.Length > 200)
                throw new System.ArgumentException(null, nameof(email));

            RoleId = roleId;
            Email = email.Trim();
            IsActive = isActive;
            UpdatedAt = now;
        }

        public void SetExternalId(string externalId, System.DateTime now)
        {
            if (externalId is null)
                throw new System.ArgumentException(null, nameof(externalId));

            if (externalId.Length > 50)
                throw new System.ArgumentException(null, nameof(externalId));

            ExternalId = externalId.Trim();
            UpdatedAt = now;
        }
    }
}