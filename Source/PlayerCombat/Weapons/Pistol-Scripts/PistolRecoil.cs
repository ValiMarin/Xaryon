using UnityEngine;

public class PistolRecoil : MonoBehaviour
{
    private Vector2 current;
    private Vector2 target;

    public Vector2 RecoilOffset => current;

    private const float RETURN = 5f;
    private const float SNAPPINESS = 30f;

    private void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsPaused)
            return;

        target = Vector2.Lerp(target, Vector2.zero, RETURN * Time.unscaledDeltaTime);
        current = Vector2.Lerp(current, target, SNAPPINESS * Time.unscaledDeltaTime);
    }

    public void ApplyRecoil(Vector2 recoil)
    {
        target += new Vector2(
            Random.Range(-recoil.x, recoil.x),
            recoil.y
        );
    }
}