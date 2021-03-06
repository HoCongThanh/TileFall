// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial  
// declarations in another file.
// </auto-generated>

using Quantum;
using UnityEngine;

[CreateAssetMenu(menuName = "Quantum/OnCollisionEnterBehaviour/VictoryAreaCollisionBehaviour", order = Quantum.EditorDefines.AssetMenuPriorityStart + 385)]
public partial class VictoryAreaCollisionBehaviourAsset : OnCollisionEnterBehaviourAsset {
  public Quantum.VictoryAreaCollisionBehaviour Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.VictoryAreaCollisionBehaviour();
    }
    base.Reset();
  }
}

public static partial class VictoryAreaCollisionBehaviourAssetExts {
  public static VictoryAreaCollisionBehaviourAsset GetUnityAsset(this VictoryAreaCollisionBehaviour data) {
    return data == null ? null : UnityDB.FindAsset<VictoryAreaCollisionBehaviourAsset>(data);
  }
}
