using System.Linq;

public class BushTorchCheck : Interactable
{
   private bool _failed;

   private Bush[] _bushes;
   private Torch[] _torches;

   private void Start()
   {
      _bushes = FindObjectsOfType<Bush>();
   }

   private void LateUpdate()
   {
      if (_failed)
         return;

      if (CaveMan.isHoldingTorch && CaveMan.isTorchLit)
         return;

      foreach (var bush in _bushes)
      {
         if (bush == null)
            continue;

         if (bush.isOnFire)
            return;
      }

      _torches = FindObjectsOfType<Torch>();
      
      if (_torches.Any(t => t.isOnFire))
      {
         return;
      }

      

      Failed();
   }

   private void Failed()
   {
      _failed = true;
      LevelManager.ForceFailed();
   }
}
