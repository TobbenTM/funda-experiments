import React, { Component } from 'react';
import Leaderboard from './components/Leaderboard';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <main>
        <h1>Funda Experiments</h1>
        <Leaderboard/>
      </main>
    );
  }
}
