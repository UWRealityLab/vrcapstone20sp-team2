﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dish_detect : MonoBehaviour
{
  private LiquidFillManager liquid;

  void OnTriggerStay(Collider other)
  {
      if (other.gameObject.tag == "Beaker")
      {
          liquid = other.transform.gameObject.GetComponent<LiquidFillManager>();
          liquid.IncubateLiquid();
      }
  }
}
