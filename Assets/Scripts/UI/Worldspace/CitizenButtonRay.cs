/**
 * @brief �ֹ� ��� ȹ�� ��ư
 * @author ��̼�
 * @date 22-04-27
 */

public class CitizenButtonRay : ButtonRaycast
{
    public RabbitCitizen citizen;

    protected override void Touched()
    {
        base.Touched();
        citizen.isTouch = true;
    }
}
