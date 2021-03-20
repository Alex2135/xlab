using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScreenController
{
    IScreenController NextScreen { get; set; }
    IScreenController PrevScreen { get; set; }
    object GetResult();
}

public class ScreensUIController : MonoBehaviour
{
    /*
     * Это главный мэнеджер всех экранов в приложении
     * он определяет какое окно за каким будет отображаться
     */

    public LoaderUiController loaderController;
    public MathTestUIController mathTestController;
    public ResultsUiController resultController;
    private List<IScreenController> screens;

    /*
     * Добавить методы для возврата результата от экрана, 
     * для передачи данных на новый экрана
     * 
     */
    private void Awake()
    {
        screens = new List<IScreenController>();
        screens.Add(loaderController);
        screens.Add(mathTestController);
        screens.Add(resultController);
        foreach (var screen in screens)
        {
            if (screen is MonoBehaviour mb)
                mb.gameObject.SetActive(false);
        }

        mathTestController.NextScreen = resultController;

        mathTestController.gameObject.SetActive(true);
    }
}
