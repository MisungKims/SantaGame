/**
 * @brief ÁÖ¹Î ´ç±Ù È¹µæ ¹öÆ°
 * @author ±è¹Ì¼º
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
