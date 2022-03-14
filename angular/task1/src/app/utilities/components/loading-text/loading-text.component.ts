import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-loading-text',
  template: `
    <div class="loading">
      <div class="loading__letter">L</div>
      <div class="loading__letter">o</div>
      <div class="loading__letter">a</div>
      <div class="loading__letter">d</div>
      <div class="loading__letter">i</div>
      <div class="loading__letter">n</div>
      <div class="loading__letter">g</div>
      <div class="loading__letter">.</div>
      <div class="loading__letter">.</div>
      <div class="loading__letter">.</div>
    </div>
  `,
  styleUrls: ['./loading-text.component.scss'],
})
export class LoadingTextComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}
