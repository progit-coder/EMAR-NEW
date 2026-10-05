import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-single-login',
  templateUrl: './single-login.component.html',
  styleUrls: ['./single-login.component.css']
})
export class SingleLoginComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {

    this.route.queryParams.subscribe(params => {

      const username = params['username'];
      const password = params['password'];
      const Nrstid = params['Nrstid'];
      const ScreenId = params['ScreenId'];
      const KeyID = params['KeyID'];

      console.log('Single Login username:', username);
      console.log('Nrstid:', Nrstid);
      console.log('ScreenId:', ScreenId);
      console.log('KeyID:', KeyID);

      if (!username || !password) {
        this.router.navigate(['/login']);
        return;
      }

      this.router.navigate(['/login'], {
        queryParams: {
          username: username,
          password: password,
          Nrstid: Nrstid,
          ScreenId: ScreenId,
          KeyID: KeyID
        }
      });

    });
  }
}
