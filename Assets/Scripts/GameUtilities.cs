using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GameUtilities : MonoBehaviour {

    //Custom Utilities class which hold imp function accross all my projects!!

    /*************************************************************************************
    *************************************   LOAD FUNCTIONS  ******************************
    *************************************************************************************/


    public static void RedirectToURL(string url) {
        Application.OpenURL(url);
    }


    /*public static void LoadScene(Scene scene) {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene);
    }*/

    public static void QuitGame() {
        Application.Quit();
    }

    /*************************************************************************************
    *************************************   TIME FUNCTIONS  ******************************
    *************************************************************************************/

    public static void PauseTime() {
        SetTimeScale(0f);
    }

    public static void ResumeTime() {
        SetTimeScale(1f);
    }

    public static void SlowMotionTime() {
        SetTimeScale(0.5f);
    }

    public static void FastForwardTime() {
        SetTimeScale(2f);
    }

    public static void SetTimeScale(float scale) {
        Time.timeScale = scale;
    }


    /*************************************************************************************
    *************************************   IMP FUNCTIONS  ******************************
    *************************************************************************************/

    public static bool IsObjectOutOfView(Vector3 position) {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(position);

        if (viewportPos.z < 0) {
            return true;
        }

        if ((viewportPos.x < 0) || (viewportPos.x > 1)) {
            return true;
        }

        if ((viewportPos.y < 0) || (viewportPos.y > 1)) {
            return true;
        }

        return false;
    }

    public static void SafeDestroyGameObject(GameObject gameobject) {
        if (gameobject != null) {
            Object.Destroy(gameobject);
        }
    }


    public static Vector3 GetMouseWorldPosition() {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;
        return Camera.main.ScreenToWorldPoint(mouse);
    }

    public static Vector3 GetMouseWorldToScreenPosition(Vector3 worldPosition) {
        return Camera.main.WorldToScreenPoint(worldPosition);
    }

    /*************************************************************************************
    *************************************   STATE FUNCTIONS  *****************************
    *************************************************************************************/

    public static bool GetCurrentGameState() {
        //return GameManager.Instance.CurrentGameState == GameState.Playing;
        return true;
    }

    public static bool GetCurrentLevelState() {
        //return GameManager.Instance.CurrentGameState == GameState.Playing;
        return true;
    }

    public static bool GetCurrentGameplayState() {
        //return GameManager.Instance.CurrentGameState == GameState.Playing;
        return true;
    }

    /*************************************************************************************
    *************************************   MATHS FUNCTIONS  *****************************
    *************************************************************************************/

    public static int AllInclusiveRandomRangeInt(int min, int max) {
        return Random.Range(min, max + 1);
    }

    public static float AllInclusiveRandomRangeFloat(float min, float max) {
        return Random.Range(min, max);
    }

    // Normalize a value between 0–1
    public static float NormalizeBetween(float value, float min, float max) {
        if (Mathf.Approximately(min, max)) {
            return 0f;
        }

        return (value - min) / (max - min);
    }

    // Get random sign (-1 or 1)
    public static int RandomSign() {
        return Random.value < 0.5f ? -1 : 1;
    }

    // Check if a value is between two numbers
    public static bool IsValueBetween(float value, float min, float max) {
        return value >= min && value <= max;
    }

    // Distance between two points
    public static float DistanceBetween(Vector2 a, Vector2 b) {
        return Vector2.Distance(a, b);
    }


}