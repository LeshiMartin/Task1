import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-not-found',
  template: `
    <div class="alert">
      <p>
        No component matches the route
      </p>
    </div>
  `,
  styles: [
    `
      .alert {
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 10px;
        background-color: #f44336 !important;
        color: #fff !important;
      }
      .alert p {
        margin: 0;
      }
    `,
  ],
})
export class NotFoundComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}
