﻿#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2019-10-18 오전 10:58:03
 *	개요 : 
   ============================================ */
#endregion Header

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 체력바, 경험치바 등에 사용하는 애니메이션 바.
/// <para>주의) 이 객체는 바로 사용할 수 없습니다.</para>
/// <para><see cref="DoAdd_AnimationLogic_OnIncrease"/>를 통해 Add하여 사용해야 합니다.</para>
/// </summary>
public class CUIAnimatedBar : MonoBehaviour, IUIWidget
{
    /* const & readonly declaration             */

    /* enum & struct declaration                */

    public enum EDirection
    {
        Increase,
        Decrease,
    }

    /* public - Field declaration            */

    public delegate void del_OnChange_FillAmount(float fFillAmount_0_1_Before, float fFillAmount_0_1_After, CUIAnimatedBar.EDirection eDirection);
    public event del_OnChange_FillAmount OnChange_FillAmount;

    public IUIManager pUIManager { get; set; }

    public Image pImage_Fill;

    /* protected & private - Field declaration         */

    List<IAnimatedBarLogic> _listLogic_OnIncrease = new List<IAnimatedBarLogic>();
    List<IAnimatedBarLogic> _listLogic_OnDecrease = new List<IAnimatedBarLogic>();

    // ========================================================================== //

    /* public - [Do] Function
     * 외부 객체가 호출(For External class call)*/

    public void DoAdd_AnimationLogic_OnIncrease(IAnimatedBarLogic pLogic, Image pImage)
    {
        pLogic.IAnimatedBarLogic_OnAwake(pImage);
        _listLogic_OnIncrease.Add(pLogic);
    }

    public void DoAdd_AnimationLogic_OnDecrease(IAnimatedBarLogic pLogic, Image pImage)
    {
        pLogic.IAnimatedBarLogic_OnAwake(pImage);
        _listLogic_OnDecrease.Add(pLogic);
    }

    public void DoSet_BarFill(float fFill_0_1)
    {
        pImage_Fill.fillAmount = fFill_0_1;
    }
    public void DoSet_BarFill_And_PlayAnimation(float fFill_0_1)
    {
        if (pImage_Fill.fillAmount < fFill_0_1)
            Set_FillAmount_OnIncrease(fFill_0_1);
        else if (pImage_Fill.fillAmount > fFill_0_1)
            Set_FillAmount_OnDecrease(fFill_0_1);
    }

    public void Do_AllClear_Listener_OnChange_FillAmount()
    {
        OnChange_FillAmount = null;
    }

    // ========================================================================== //

    /* protected - Override & Unity API         */

    void Update()
    {
        float fDeltaTime = Time.deltaTime;
        for (int i = 0; i < _listLogic_OnIncrease.Count; i++)
            _listLogic_OnIncrease[i].IAnimatedBarLogic_OnUpdate(fDeltaTime);

        for (int i = 0; i < _listLogic_OnDecrease.Count; i++)
            _listLogic_OnDecrease[i].IAnimatedBarLogic_OnUpdate(fDeltaTime);
    }

    public IEnumerator OnShowCoroutine()
    {
        yield break;
    }

    public IEnumerator OnHideCoroutine()
    {
        yield break;
    }

    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    void Set_FillAmount_OnIncrease(float fFillAmount_0_1)
    {
        float fBeforeFill = pImage_Fill.fillAmount;
        pImage_Fill.fillAmount = fFillAmount_0_1;

        for (int i = 0; i < _listLogic_OnIncrease.Count; i++)
            _listLogic_OnIncrease[i].IAnimatedBarLogic_OnStartAnimation(fBeforeFill, fFillAmount_0_1, EDirection.Increase);

        OnChange_FillAmount?.Invoke(fBeforeFill, fFillAmount_0_1, EDirection.Increase);
    }

    void Set_FillAmount_OnDecrease(float fFillAmount_0_1)
    {
        float fBeforeFill = pImage_Fill.fillAmount;
        pImage_Fill.fillAmount = fFillAmount_0_1;

        for (int i = 0; i < _listLogic_OnDecrease.Count; i++)
            _listLogic_OnDecrease[i].IAnimatedBarLogic_OnStartAnimation(fBeforeFill, fFillAmount_0_1, EDirection.Decrease);

        OnChange_FillAmount?.Invoke(fBeforeFill, fFillAmount_0_1, EDirection.Decrease);
    }

    #endregion Private
}