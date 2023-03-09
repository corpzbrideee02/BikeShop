<template>
  <div class="container">
    <img
      style="margin-top: 2vh"
      src="../assets/orderLogo.jpg"
      height="100"
    />
    <div class="heading">Your Orders</div>
    <div class="status">{{ state.status }}</div>
    <div>  
      <!-- DATATABLE -->
      <DataTable
        v-if="state.orders.length > 0"
        :value="state.orders"
        :scrollable="true"
        scrollHeight="60vh"
        dataKey="id"
        class="p-datatable-striped"
        v-model:selection="state.orderselected"
        selectionMode="single"
        @row-select="onRowSelect"
      >
        <Column header="Order #" field="id" />
        <Column header="Date">
          <template #body="slotProps">
            {{ formatDate(slotProps.data.orderDate) }}
          </template>
        </Column>
      </DataTable>

      <!-- DIALOG -->
      <Dialog v-model:visible="state.selectedOrder" class="dialog-border">
        <div class="orderNum">Order#: {{ state.orderDetails[0].orderId }}</div>
        <div class="orderDate">{{ state.orderDetails[0].orderDate }}</div>
        <div style="text-align: center; margin: 3vh">
          <img height="100" src="../assets/orderLogo.jpg" />
        </div>
        <div class="order-head">Quantities</div>
        <DataTable
          :value="state.orderDetails"
          :scrollable="true"
          scrollHeight="38vh"
          dataKey="id"
          class="p-datatable-striped"
          style="min-width: 20vw"
        >
          <Column header="Name" field="productName" style="min-width: 20vw" />
          <Column header="S" field="qtySold" />
          <Column header="O" field="qtyOrdered" />
          <Column header="B" field="qtyBackOrdered" />
          <Column header="Extended" field="sellingPrice">
            <template #body="slotProps">
              {{ formatCurrency(slotProps.data.sellingPrice) }}
            </template>
          </Column>
        </DataTable>

        <div v-if="state.orders.length > 0">
          <table style="margin-top: 1vh">
            <tr>
              <td style="width: 100%; text-align: right; font-weight: bold">
                Sub Total:
              </td>
              <td style="text-align: right; padding: 0 3vw">
                {{ formatCurrency(state.orderselected.orderAmount) }}
              </td>
            </tr>

            <tr>
              <td style="width: 100%; text-align: right; font-weight: bold">
                Tax:
              </td>
              <td style="text-align: right; padding: 0 3vw">
                {{ formatCurrency(getTax(state.orderselected.orderAmount)) }}
              </td>
            </tr>
            <tr>
              <td style="width: 100%; text-align: right; font-weight: bold">
                Total:
              </td>
              <td style="text-align: right; padding: 0 3vw">
                {{ formatCurrency(finalTotal(state.orderselected.orderAmount)) }}
              </td>
            </tr>
          </table>
        </div>
      </Dialog>
    </div>
  </div>
</template>
<script>
import { reactive, onMounted } from "vue";
import { fetcher } from "../util/apiutil";
export default {
  setup() {
    let state = reactive({
      status: "",
      dialogStatus: "",
      orders: [],
      selectedOrder: false,
      orderselected: {},
      orderDetails: [],
    });
    onMounted(() => {
      loadOrders();
    });
    const loadOrders = async () => {
      try {
        let customer = JSON.parse(sessionStorage.getItem("customer"));
        const payload = await fetcher(`order/${customer.email}`);
        if (payload.error === undefined) {
          state.orders = payload;
          state.status = `loaded ${state.orders.length} orders`;
          state.selectedOrder = false;
        } else {
          state.status = payload.error;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    const formatDate = (date) => {
      let d;
      // see if date is coming from server
      date === undefined ? (d = new Date()) : (d = new Date(Date.parse(date))); // from server
      let _day = d.getDate();
      let _month = d.getMonth() + 1;
      let _year = d.getFullYear();
      let _hour = d.getHours();
      let _min = d.getMinutes();
      if (_min < 10) {
        _min = "0" + _min;
      }
      return _year + "-" + _month + "-" + _day+" "+_hour+":"+_min;
    };

    const onRowSelect = async (event) => {
      try {
        let customer = JSON.parse(sessionStorage.getItem("customer"));
        state.orderselected = event.data;
        const payload = await fetcher(
          `order/${state.orderselected.id}/${customer.email}`
        );
        state.orderDetails = payload;
        state.dialogStatus = `details for tray ${state.orderselected.id}`;
        state.selectedOrder = true;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    const formatCurrency = (value) => {
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    };
    const getTax = (value) => {
      value = state.orderselected.orderAmount* 0.13;
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    };
    const finalTotal = (subt) => {
      return subt + subt * 0.13;
    };
    return {
      state,
      formatDate,
      onRowSelect,
      formatCurrency,
      getTax,
      finalTotal,
    };
  },
};
</script>
