using UnityEngine;


public class S_Pause : MonoBehaviour
{
    // Start is called before the first frame update

    //�|�[�Y��ʂ̃v���n�u�������Ă���
    [SerializeField] private GameObject pauseUI;

    //�|�[�Y��ʒ����m�F(true�Ń|�[�Y��)
    private bool bNowPause = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //�L�[��������bNowPause�𔽓]
        if (Input.GetKeyDown(KeyCode.Escape))
            bNowPause = !bNowPause;

       //bNowPause�ɉ����ă|�[�Y��ʂɕύX���邩���߂�
        if (bNowPause)
            Pause();
        else
            GameScene();
    }

    private void FixedUpdate()
    {
        //Debug.Log("�o�ߎ���(�b)" + Time.time);
    }

    private void Pause()
    {
        //Pause��ʂ��\������Ă��邩�m�F����B
        if (!pauseUI.activeSelf)
        {
            //�A�N�e�B�u�ɕύX
            pauseUI.SetActive(true);
            //�Q�[���̎��Ԃ��~�߂�
            Time.timeScale = 0f;
        }

    }

   private void GameScene()
    {
        //Pause��ʂ��\������Ă������\���ɂ���
        if (pauseUI.activeInHierarchy)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }


    //Pause���A���X�^�[�g�{�^���������ꂽ�Ƃ���bNowPause��false�ɂ���
    public void ReStart()
    {
        if (bNowPause)
            bNowPause = false;
    }

}
