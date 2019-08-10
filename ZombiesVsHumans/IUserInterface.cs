namespace ZombiesVsHumans
{
    public interface IUserInterface
    {
         void ShowError(string msg);

         void RenderWorld(Agent[,] world);
    }
}