namespace Runtime.ETC
{
    public enum PlayerState
    {
        Idle,
        Move,
        Interact,
    }

    public enum JewelryType
    {
        A,
        B,
        C,
        None
    }

    public enum Speaker
    {
        dustA,
        dustB,
        rapley,
        none
    }

    public enum Sound
    {
        BGM,
        Effect,
        Speech,
        Max,
    }

    //Chapter 관리를 위한 임시 데이터
    public enum Chapter
    {
        CHAP1 = 100,
        CHAP1_PM = 110,
        CHAP2_TMP = 120,
        CHAP1_SLG = 130,
        CHAP2 = 200,
        Max
    }

    public enum SLGProgress
    {
        None = 0,
        ModeOpen, //SLG 처음 모드 오픈
        BeforeConstruction, //건설 시작 전
        Constructing, //건설중 (가속 가능)
        EndConstruction,
        ModeClose, //SLG 모드 끝남
        Max
    }
}