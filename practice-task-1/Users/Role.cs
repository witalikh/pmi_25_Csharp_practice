namespace practice_task_1;

public class Role
{
    public bool IsSuperAdmin { get; } = false;
    public bool HasApprovalPerms { get; } = false;
    public bool HasEditPermissions { get; } = false;
    public bool HasEditPermissionsForOtherInstances { get; } = false;

    public Role(
        bool is_superadmin,
        bool has_approval_perms,
        bool has_edit_perms,
        bool has_edit_perms_for_other_instances)
    {
        this.IsSuperAdmin = is_superadmin;
        this.HasApprovalPerms = has_approval_perms;
        this.HasEditPermissions = has_edit_perms;
        this.HasEditPermissionsForOtherInstances = has_edit_perms_for_other_instances;
    }
}

public static class MainRoles
{
    public static Role Admin = new(
        is_superadmin: false, 
        has_approval_perms: true, 
        has_edit_perms: true, 
        has_edit_perms_for_other_instances: true
        );
    
    public static Role Staff = new(
        is_superadmin: false, 
        has_approval_perms: false, 
        has_edit_perms: true, 
        has_edit_perms_for_other_instances: false
        );

    public static readonly AnonymousUser AnonymousUser = new();
}