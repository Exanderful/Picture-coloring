namespace echo17.EndlessBook.Demo02
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.EndlessBook;
    using TMPro;

    /// <summary>
    /// Table of contents page.
    /// Handles clicks on the chapters to jump to pages in the book
    /// </summary>
    public class PageView_Text : PageView
    {
        public TextMeshPro Text;

        private string text = "Покорение змея Калия\nВ этой истории Кришна гулял со своими друзьями пастушками и телятами рядом с заводью реки Ямуны, в которой обитал злобный ядовитый змей Калия";

        private void Started()
        {
            //StartCoroutine("showText", text);
        }

        private void OnEnable()
        {
            Started();
        }

        IEnumerator showText(string text)
        {
            int x = 0;
            while(x<= text.Length)
            {
                Text.text = text.Substring(0, x);
                x++;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
