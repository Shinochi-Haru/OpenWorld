using UnityEngine.UI;

public class DiaController : ItemController
{
    // カウントを表示するUIのテキストオブジェクト
    public Text countText;

    // アイテムのカウント
    private int itemCount = 0;

    // オーバーライドしたUseメソッド
    public override void Use()
    {
        // 親クラスのUseメソッドを呼び出す
        base.Use();

        // カウントを増やす
        itemCount++;

        // カウントを更新して表示する
        countText.text = itemCount.ToString();
    }
}
