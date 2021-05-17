namespace MRL.SSL.Ai.Engine
{
    public class RoleInfo
    {
        public RoleBase Role { get; set; }
        public float Weight { get; set; }
        public float Margin { get; set; }

        public RoleInfo(RoleBase role, float weight, float margin)
        {
            Role = role;
            Weight = weight;
            Margin = margin;
        }
    }
}