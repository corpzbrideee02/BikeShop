<template>
  <div>
    <img src="../assets/bike.png" height="100" />
    <div class="heading">3 Closest Branches</div>
    <div>
      <InputText
        placeholder="enter address"
        id="address"
        v-model="state.address"
        style="width: 40vw; margin-right: 2vw; padding: 10px"
      />
      <Button
        @click="getLatLon"
        class="pi pi-search"
        style="
          width: 10vw;
          color: var(--teal-900);
          background-color: white;
          border: 1px var(--teal-900) solid;
        "
      />
    </div>
    <br />

    <!-- <p></p>
    {{ state.latlon }} -->
    <!-- Warning and Error Message -->
    <Message
      severity="warn"
      v-if="state.showWarning == true"
      :life="3000"
      :sticky="false"
      >Warning: Please Enter Address</Message
    >
    <Message
      severity="error"
      v-if="state.showError == true"
      :life="3000"
      :sticky="false"
      >Error: Invalid Address</Message
    >
    <div
      id="map"
      ref="mapRef"
      v-show="
        state.showmap === true &&
        state.showError === false &&
        state.showWarning === false
      "
    ></div>

    <div class="clear"></div>
    <!-- Warning and Error Message -->
  </div>
</template>
<script>
import { ref, reactive } from "vue";

import { fetcher } from "../util/apiutil";
export default {
  name: "Map",
  setup() {
    const mapRef = ref(null);
    let state = reactive({
      status: "",
      address: "",
      latlon: "",
      showmap: false,
      showWarning: false,
      showError: false,
    });
    const getLatLon = async () => {
      state.showWarning = false;
      state.showError = false;

      //check first if the user enter something

      if (state.address.length == 0) {
        state.showWarning = true;
      }

      try {
        const tt = window.tt;
        let url = `https://api.tomtom.com/search/2/geocode/${state.address}.json?key=HZTNMJGyvobd3HhGaZd14rGnpLPwv0mL `;
        let response = await fetch(url);

        let payload = await response.json();
        let lat = payload.results[0].position.lat;
        let lon = payload.results[0].position.lon;
        state.latlon = `Address is at Lat ${lat} and Lon ${lon} `;
        let map = tt.map({
          key: "HZTNMJGyvobd3HhGaZd14rGnpLPwv0mL",
          container: mapRef.value,
          source: "vector/1/basic-main",
          center: [lon, lat],
          zoom: 8,
        });
        map.addControl(new tt.FullscreenControl());
        map.addControl(new tt.NavigationControl());

        let threeBranches = await fetcher(`branch/${lat}/${lon}`);
        threeBranches.map((branch) => {
          let marker = new tt.Marker()
            .setLngLat([branch.longitude, branch.latitude])
            .addTo(map);
          let popupOffset = 25;
          let popup = new tt.Popup({ offset: popupOffset });
          popup.setHTML(
            `<div id="popup">Branch#: ${branch.id}</div><div>${
              branch.street
            }, ${branch.city}<br/>${branch.distance.toFixed(2)} km <div>`
          );
          marker.setPopup(popup);
        });
        state.showmap = true;
      } catch (err) {
        state.status = err.message;
        if (state.address.length > 0) {
          state.showError = true;
        }
      }
    };
    return {
      mapRef,
      state,
      getLatLon,
    };
  },
};
</script>
<style>
#map {
  border: 2px solid;
}
#popup {
  font-weight: bold;
}
@media (max-width: 480px) {
  #map {
    height: 40vh;
    width: 90vw;
    margin: auto;
  }
}
/* tablets and small laptops */
@media only screen and (max-width: 768px) and (min-width: 481px) {
  #map {
    height: 40vh;
    width: 80vw;
    margin: auto;
  }
}

/* larger screen */
@media (min-width: 769px) {
  #map {
    height: 50vh;
    width: 70vw;
    margin: auto;
  }
}
</style>
